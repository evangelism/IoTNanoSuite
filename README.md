# IoTNanoSuite

**IoT NanoSuite** is designed as a sample IoT system that demonstrates the usage of Microsoft Azure for light-weight real-time ingestion and processing of messages from a number of sensors. It uses **Azure Functions** for doing real-time data processing, which is very cost-efficient way, yet it offers quite a lot of flexibility for message processing and automatic scaling (serverless compute). 

## What is included

The package consists of the following projects:
* [AzureFunction](AzureFunction) - the code of the Azure Function that is deployed to the cloud to do message processing
* [IoTNanoSuite/DeviceEmulator](IoTNanoSuite/DeviceEmulator) - Device Emulator that can be used to send sample messages to the IoT Hub. The emulator is UWP application, and can be deployed on any Windows Device, including **Raspberry Pi** running Windows IoT Suite.
* [IoTNanoSuite/WebPortal](IoTNanoSuite/WebPortal) - simple web application that displays the data in graphical form.
* [Interpolator](Interpolator) - sample console application that tests data generation and interpolation algorithm in console. When the algorithm is debugged there, we can then cut-and-paste it into Azure Function.

To create the working solution, you would need:
* Create IoT Hub (or Event Hub) in the Azure Portal
* Create an Azure Function and upload  