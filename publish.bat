pushd %1
del publish\*.* /q
dotnet publish -o publish -c %2
ssh pi@raspberrypi rm -fr pi-hifi-control/*
scp publish\* pi@raspberrypi:pi-hifi-control
popd
