# pi-hifi-control

## Raspberry Pi Configuration

### GPIO Pinout

![GPIO Pinout](https://www.raspberrypi.com/documentation/computers/images/GPIO-Pinout-Diagram-2.png)

### Serial HAT

Requires **GPIO 14 and 15** (pins 8 and 10). See [here](https://thepihut.com/blogs/raspberry-pi-tutorials/how-to-use-the-modmypi-serial-hat) for configuration details.

### IR Blaster

Edit **/boot/config.txt** and uncomment:

```
dtoverlay=gpio-ir-tx,gpio_pin=18
```

See [here](https://github.com/gordonturner/ControlKit/blob/master/Raspbian%20Setup%20and%20Configure%20IR.md) for configuration details
and [here](https://blog.gordonturner.com/2020/05/31/raspberry-pi-ir-receiver/) for circuit diagrams.

## Web Site

### Certificate

```bash
sudo openssl req -x509 -nodes -days 3650 -newkey rsa:2048 -keyout /etc/ssl/private/web-selfsigned.key -out /etc/ssl/certs/web-selfsigned.crt
```

### Start

```bash
docker run --rm -it -p 80:80 -p 443:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/web-selfsigned.crt -e ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/keys/web-selfsigned.key -v /etc/ssl/certs/:/https/ -v /etc/ssl/private/:/keys/ --device /dev/lirc0 --device /dev/serial0 oatkins/pihificontrolblazorserver:latest
```
