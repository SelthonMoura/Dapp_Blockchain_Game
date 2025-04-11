import subprocess
import sys
import time

def run_node(port):
    return subprocess.Popen(
        [sys.executable, "-m", "uvicorn", "blockchain_enhanced_full:app", "--reload", "--port", str(port)],
        cwd=".",  # ou o path correto se o script não estiver no mesmo diretório
    )

if __name__ == "__main__":
    ports = [8000, 8001, 8002]
    processes = []

    for port in ports:
        print(f"Iniciando nó na porta {port}...")
        p = run_node(port)
        processes.append(p)
        time.sleep(1)

    print("Todos os nós foram iniciados. Pressione Ctrl+C para encerrar.")

    try:
        for p in processes:
            p.wait()
    except KeyboardInterrupt:
        print("Encerrando nós...")
        for p in processes:
            p.terminate()
