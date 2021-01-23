pushd %1
md publish
del publish\*.* /q
dotnet publish .\src\Pi.HifiControl\pi-hifi-control.csproj -r linux-arm -o publish -c %2
ssh pi@raspberrypi rm -fr pi-hifi-control/*
scp publish\* pi@raspberrypi:pi-hifi-control
ssh pi@raspberrypi chmod 777 pi-hifi-control/pi-hifi-control
popd
