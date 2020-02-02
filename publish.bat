pushd %1
md publish
del publish\*.* /q
dotnet publish .\src\Pi.HifiControl\pi-hifi-control.csproj -o publish -c %2
ssh pi@raspberrypi rm -fr pi-hifi-control/*
scp publish\* pi@raspberrypi:pi-hifi-control
popd
