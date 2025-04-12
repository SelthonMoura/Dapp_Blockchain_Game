import time
from block_class import Block, Transaction


class Blockchain:
    def __init__(self, difficulty=2):
        self.difficulty = difficulty
        self.blocks = []
        self.transaction_pool = []
        self.create_genesis_block()

    def create_genesis_block(self):
        genesis = Block(0, time.time(), None, [])
        genesis.proof_of_work(self.difficulty)
        self.blocks.append(genesis)

    def latest_block(self):
        return self.blocks[-1]

    def add_transaction(self, transaction):
        self.transaction_pool.append(transaction)

    def mine_block(self):
        new_block = Block(
            index=self.latest_block().index + 1,
            timestamp=time.time(),
            previous_hash=self.latest_block().hash,
            transactions=self.transaction_pool.copy()
        )
        new_block.proof_of_work(self.difficulty)
        self.blocks.append(new_block)
        self.transaction_pool.clear()
        return new_block

    def is_chain_valid(self):
        return validate_chain(self.blocks, self.difficulty)

    def save(self):
        # Placeholder para persistência futura (JSON/SQLite)
        pass

def validate_chain(chain: list, difficulty: int) -> bool:
    for i in range(1, len(chain)):
        current = chain[i]
        previous = chain[i - 1]

        if current.hash != current.calculate_hash():
            return False

        if current.previous_hash != previous.hash:
            return False

        if not current.hash.startswith("0" * difficulty):
            return False

        for tx in current.transactions:
            if not tx.is_valid():
                return False

    return True
