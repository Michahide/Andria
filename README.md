# Andria

## How to Run the Project
1. Open Unity, click Add, choose directory folder.
2. Open Command Prompt at directory folder, run this command.

```bash
py -m venv venv
venv\scripts\activate
py -m pip install --upgrade pip
pip install mlagents
pip install torch torchvision torchaudio
pip install protobuf==3.19.6
pip install onnx==1.12
pip install tensorflow<2.11
mlagents-learn -h
```

3. After everything has been installed, at virtual environment, run this command with format mlagents-learn <trainer-config-file> --env=<env_name> --run-id=<run-identifier>, for example:

```bash
mlagents-learn --run-id="Test1"
```

4. Click play button at Unity to start learning process.
5. You can see the progress at your  terminal.
6. If you want to stop the process, at your terminal press Ctrl + C.
7. The result of learning can be viewed at results folder. You can also use onnx file as model at your next learning process.

## FAQ
**Q: What Python version used?**\
A: Python 3.9.13. Python 3.10.x and above cannot use Numpy that will be used for ML-Agents.

**Q: Why we need protobuf version 3.19.6?**\
A: 3.20 above cannot be used by TensorFlow 2.10.\

**Q: Why we need ONNX 1.12?**\
A: This version is last compatible with protobuf 3.19.6 .\

**Q: Why we need Tensorflow 2.11 lower (2.10)?**\
A: This version is last support training using GPU.\

## Vector Observation
| Parameter | Description |
| --- | --- |
| Vector Obeservation Space Size | 3 |  
| Stacked Vector | 3 |  

## Actions (Element and ML)  
| Parameter | Description |
| --- | --- |
| Action Space Type | Discrete |
| Discrete Branch |  7 |
| 0 | Physical Attack |  
| 1 | Guard |  
| 2 | Ice Attack |  
| 3 | Earth Attack |  
| 4 | Wind Attack |  
| 5 | Ramuan Mujarab |  
| 6 | Ramuan Pemula |  

## Actions (Non Element and ML)  
| Parameter | Description |
| --- | --- |
| Action Space Type | Discrete |
| Discrete Branch |  3 |
| 0 | Physical Attack |  
| 1 | Guard |  
| 2 | Hempasan Ratu |  

