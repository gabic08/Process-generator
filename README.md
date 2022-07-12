# Process generator
/n
This program simulates the creation of processes in an operating system, as well as their running at "the same time" using a multitasking simulation.
/n
When creating a process, it is added to a queue of ready processes.
Each process has a lifespan that is consumed when the GPU takes it.
/n
At every quantum of time, a process is taken from the queue of ready processes, it is placed in a queue of running processes (there was no need for a queue of running process because only one process can be running at a time, so the queue will always have only one object in it, so I was complicated in vain to always add and remove from this queue :))  ), the duration of the quantum is deducted from the lifespan of the process, and if it does not remain lifeless, it is placed in the queue of ready processes.
/n
In the pdf file is a report that displays how the simulation behaves when different values are entered from the keyboard
