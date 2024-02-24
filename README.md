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
pip install protobuf==3.20.3
pip install onnx
mlagents-learn -h
```

3. After everything has been installed, at virtual environment, run this command with format mlagents-learn --run-id="Name ML Agents Learn Process", for example:

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

**Q: Why we need protobuf version 3.20.3?**\
A:  The upper version (3.21 or above) is not supported.
