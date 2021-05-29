## Insurance.Api
# Design Goal:
-	separation of concern.
- Leverages SOLID principles.
- Separate individual rules from rules processing logic.
- Allows new rules to be added without the need for changes in the rest of the system.
- This aligns closely with Open/Closed principle:
- As this means we could change the behavior of the system without changing the code itself.
- It also aligns well with “Single Responsibility principle” (class or a method should have one reason to change)
- we now separate the rules from the rules processing logic as they are separate concerns so if we change how we process rules in general, that should go in rule processing class or method, however if we change the individual behavior of a particular rule, this should go inside the rule class.
- Adding business rules in app through configuration file.
- Rules could be dynamic, but we need to thing about how we will persist the rules and how we need user interface to allow them to be edited.

## Cross cutting concerns level:

Asp.net core configuration:
Read configuration using Named option pattern through IOptionsMonitor interface (more details should be added)
Named Option pattern is quite useful in situations like the one we have in the code (API’s Uri) as we have common configuration properties and avoid repeating those properties across multiple option classes.
Asp.net core Logging
resilience and transient fault handling
- Using Polly library:
o	Polly is a library that will enable us to express resilience and transient fault handling policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.
- In .net core, there is a support for using Polly with the Http factory feature which will allow us to use Polly in a loosely coupled way by registering our policies and wrap it in our HTTP requests.

### Polly Benefits:
	Bulkhead isolation:
It limits the number of requests to the remote service that can execute in parallel and limits the number of requests that can sit in a queue awaiting an execution slot 
Benefits:
- Isolation
Prevent overloading:
If one part of your application becomes overloaded, it could bring down other parts but by using Bulkhead isolation policy you could prevent this from happening or delay it.
- Resource allocation:
By allocate execution slots and queues as you see fit for your case.
- Scaling:
You can determine the number of ongoing parallel requests. You can also determine the number of requests waiting in the queue. When they reach some limit, you could trigger horizontal scaling.
- Load shedding:
As it is better to fail fast than to fail unpredictably so when your application is being overwhelmed, it will at some unknown point being slow and fail.
So you could handle this by setting when your application stops accepting requests and immediately return an error to the caller
- Timeout:
The Timeout policy lets us decide how long any request should take. If the request takes longer than specified, the policy will terminate the request and cleanup resources via the usage of a cancellation token.
- Wait and Retry:
The Wait and Retry policy let you pause before retrying, a great feature for scenarios where all you need is a little time for the problem to resolve. 
- Circuit Breaker:
If a method we are calling or a piece of infrastructure you depend on becomes very unreliable, the best thing to do might be to stop making requests to it for a moment. The Circuit Breaker policy lets you do this.
- Fallbacks:
let us return some default or perform an action like paging scaling a system or restarting a service.
To achieve all the benefits, I use Policy Wraps  

It allows any number of policies to be chained together. In my implementation, it works like this:
 fallback => circuit breaker=> wait and retry => timeout => Bulkhead
- Health check is supported:
- Is a middleware that has been a recent addition to .net core since asp .net core 2.2 for reporting the health of the asp .net core-based application.
- I am using the “Liveness health check”:
It is the simplest type of health check because it only can report that the application is running.
