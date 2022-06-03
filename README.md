# VendingMachine
This project was created for a technical interview case. The task was to improve an existing code for a vending machine implementation. 

## Future Work
This code was implemented in a limited time period and therefore the code could have some improvements. Some patterns to consider to implement for future work are listed below. 
- [State pattern](https://refactoring.guru/design-patterns/state) for making code more open for extension and closed for modification, and move state-related to logic to separate classes (single responsibility principle).
- [Chain of responsibility pattern](https://refactoring.guru/design-patterns/chain-of-responsibility) for handling different payment requests (cash, card, SMS..). 
- [Command pattern](https://refactoring.guru/design-patterns/command) for handling inputs from user.
- [Abstract factory pattern](https://refactoring.guru/design-patterns/abstract-factory) pattern for creating vending machine items (and commands if using command pattern).

Please note that these patterns might be overkill for the current implementation, but might benefit it the vending machine should scale up in the future (using different payment methods, offering a variety of different type of items, adding a authentication layer etc..).

## Code Overview
This project conists of four projects. 

- Vendee.VendingMachine.Console - Used for interacting with the vending machine.
- Vendee.VendingMachine.SmsSystem - Used for simulating sending text messages to the machine.
- Vendee.VendingMachine.UnitTests - Used to indicate if everything is working correctly.
- Vendee.VendingMachine.Core - Core classes, models, interfaces..

### Vendeelicious

![Vendeelicious](https://user-images.githubusercontent.com/98617835/167498242-dd12d4a4-0a6a-4819-b3a5-a7530bfc78ae.png)

Vendeelicious is the main project containing all the logic for the vending machine. To use SMS feature, the 'Vendee SMS System' project must be run simultanously.

### Vendee SMS System
![Vendee SMS System](https://user-images.githubusercontent.com/98617835/167498246-413cf579-9d27-4d31-9be8-d8fc46745bb1.png)

Vendee SMS System is using ZeroMQ for communicating with the vending machine. ZeroMQ is a brokerless library, and in this project used for request-response pattern.
