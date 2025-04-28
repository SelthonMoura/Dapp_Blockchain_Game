from fastapi import FastAPI
from pydantic import BaseModel
from starlette.responses import FileResponse

from block_class import Block, Transaction
from blockchain import Blockchain, validate_chain

from ecdsa import SigningKey, NIST384p
import requests

app = FastAPI()
blockchain = Blockchain(difficulty=2)
peers = set()

import json, os
PORT = os.getenv("PORT", "8000")

@app.get("/")
def root():
    return {"message": "API funcionando"}

@app.get("/webui.html")
def get_webui():
    return FileResponse("webui.html")

@app.get("/generate_keys")
def generate_keys():
    sk = SigningKey.generate(curve=NIST384p)
    vk = sk.verifying_key
    return {
        "private_key": sk.to_pem().decode(),
        "public_key": vk.to_pem().decode()
    }

class SignData(BaseModel):
    tx_data: str
    private_key: str

@app.post("/sign")
def sign_data(data: SignData):
    sk = SigningKey.from_pem(data.private_key.encode())
    signature = sk.sign(data.tx_data.encode())
    return {"signature": signature.hex()}

class TransactionData(BaseModel):
    player: str
    item: str
    amount: int
    signature: str
    public_key: str

@app.post("/transaction")
def add_transaction(tx: TransactionData):
    transaction = Transaction(**tx.dict())
    if not transaction.is_valid():
        return {"detail": "Transação inválida"}, 400
    blockchain.add_transaction(transaction)

    return {"message": "Transação adicionada"}

@app.post("/mine")
def mine():
    if not blockchain.transaction_pool:
        return {"message": "Nenhuma transação para minerar"}
    block = blockchain.mine_block()
    return {"message": "Bloco minerado", "block": block.to_dict()}

@app.get("/chain")
def get_chain():
    return [block.to_dict() for block in blockchain.blocks]

@app.get("/check_chain")
def check_chain():
    if blockchain.is_chain_valid():
        return {"valid": True}
    return {"valid": False, "message": "Blockchain foi corrompida!"}

@app.get("/hack_block")
def hack_block():
    if len(blockchain.blocks) > 1:
        blockchain.blocks[1].transactions[0].amount = 9999
        blockchain.blocks[1].hash = blockchain.blocks[1].calculate_hash()
        return {"message": "Bloco adulterado com sucesso"}
    return {"message": "Não há bloco suficiente para adulterar."}

@app.get("/export")
def export_chain():
    return [block.to_dict() for block in blockchain.blocks]

@app.post("/import")
def import_chain(received_chain: list):
    new_chain = []
    for b in received_chain:
        transactions = [Transaction(**tx) for tx in b["transactions"]]
        new_block = Block(
            index=b["index"],
            timestamp=b["timestamp"],
            previous_hash=b["previous_hash"],
            transactions=transactions,
            nonce=b.get("nonce", 0)
        )
        new_chain.append(new_block)
    blockchain.blocks = new_chain
    blockchain.save()
    return {"message": "Blockchain importada com sucesso"}

@app.post("/peers/register")
def register_peer(peer_url: str):
    peers.add(peer_url)
    return {"peers": list()}

@app.get("/peers")
def list_peers():
    return {"peers": list(peers)}

@app.get("/peers/sync")
def sync_with_peers():
    global blockchain
    from copy import deepcopy

    print(f"{PORT} adicionando peers")
    # Adiciona os outros peers
    with open("peers.json") as f:
        peer_list = json.load(f)
        for p in peer_list:
            if not peers.__contains__(p):
                if p != f"http://localhost:{PORT}":
                    try:
                        peers.add(p)
                    except:
                        print(f"Não conseguiu registrar o peer {p}")

    # Primeiro, valida a própria cadeia
    current_chain = blockchain.blocks
    valid_chains = []

    if validate_chain(current_chain, blockchain.difficulty):
        valid_chains.append((len(current_chain), deepcopy(current_chain), "local"))

    for peer in peers:
        try:
            response = requests.get(f"{peer}/export", timeout=5)
            peer_chain_data = response.json()

            # Reconstrói a cadeia do peer
            peer_chain = []
            for b in peer_chain_data:
                transactions = [Transaction(**tx) for tx in b["transactions"]]
                block = Block(
                    index=b["index"],
                    timestamp=b["timestamp"],
                    previous_hash=b["previous_hash"],
                    transactions=transactions,
                    nonce=b["nonce"]
                )
                peer_chain.append(block)

            if validate_chain(peer_chain, blockchain.difficulty):
                valid_chains.append((len(peer_chain), peer_chain, peer))

        except Exception as e:
            print(f"Erro ao conectar com peer {peer}: {e}")
            continue

    if not valid_chains:
        return {"message": "Nenhuma cadeia válida encontrada entre peers ou local."}

    # Seleciona a cadeia válida mais longa
    valid_chains.sort(key=lambda x: x[0], reverse=True)
    best_length, best_chain, source = valid_chains[0]

    if source != "local":
        blockchain.blocks = deepcopy(best_chain)
        blockchain.save()
        return {"message": f"Blockchain sincronizada com sucesso a partir de '{source}'", "length": best_length}

    return {"message": "Nenhuma cadeia mais longa encontrada."}
