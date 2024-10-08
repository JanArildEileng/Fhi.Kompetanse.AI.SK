 
 #1 OLLMA download and create container
 podman run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama



 
 http://localhost:11434/


 https://ollama.com/
 https://github.com/ollama/ollama/blob/main/docs/api.md

 #run interacktiv with model
 podman exec -it ollama ollama run llama3

 #ikke bruk: run container
 # podman exec ollama ollama run llama3

 #2 OPEN WBEBUI
 podman run -d -p 3000:8080 -e WEBUI_AUTH=False --add-host=host.docker.internal:host-gateway -v  open-webui:/app/backend/data --name open-webui --restart always ghcr.io/open-webui/open-webui:main
 

 http://localhost:3000/
 http://localhost:3000/ollama/docs
 https://docs.openwebui.com/api/#swagger-documentation-links


#3 PODMAN GPU KONFIGURERING

3.1)
podman machine ssh

3.2)
#curl -s -L https://nvidia.github.io/libnvidia-container/stable/rpm/nvidia-container-toolkit.repo | \
  sudo tee /etc/yum.repos.d/nvidia-container-toolkit.repo
# sudo yum install -y nvidia-container-toolkit
# sudo nvidia-ctk cdi generate --output=/etc/cdi/nvidia.yaml
# nvidia-ctk cdi list

3.3)
podman run --rm --device nvidia.com/gpu=all --security-opt=label=disable ubuntu nvidia-smi -L


//START OLLMA med GPU  (kun hvis GPU støtte er konfiguert som over)
podman run -d --gpus=all  -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama