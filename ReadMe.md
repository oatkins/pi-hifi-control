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
