# Process Manager as a state machine implemented with stateless nuget package and entity framework core

Process manager maintains schema for transitions between states and implements state design pattern.  

In the code example we have user which can request for credit offer, accept/decline it and pay balance. Those actions are triggers which trigger states transitions.  
CardStateMachineExtension.cs file contains schema on transitions between states triggered by actions.  
https://github.com/dekizard/ProcessManager/blob/master/ProcessManager/Application/ProcessManager/CardStateMachineExtension.cs

The next transitions are valid:  
Init->RequireCreditOfferSuccess  
Init->RequireCreditOfferFailed  
RequireCreditOfferSuccess->AcceptCreditOfferSuccess  
RequireCreditOfferSuccess->AcceptCreditOfferFailed  
RequireCreditOfferSuccess->DeclineCreditOfferSuccess  
RequireCreditOfferSuccess->DeclineCreditOfferFailed  

The next states are substates of Init state.  
RequireCreditOfferFailed  
AcceptCreditOfferFailed  
DeclineCreditOfferFailed  

User can pay balance only if card is in AcceptCreditOfferSuccess state. Also user can't decline or initialize credit offer if credit offer is already approved.
Failed statuses (when out of process call fails) are configured as substates of Init state and play role as starting point for requiring a new credit offer.  

Stateless nuget package:
https://github.com/dotnet-state-machine/stateless
