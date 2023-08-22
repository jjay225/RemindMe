# RemindMe
Simple email reminder service running as a container instance in Azure with Docker support.

## How does it work

So you don't need to run this as a container or on Azure, just happens to be my use case. The runable project is **_RemindMe.Reminder_**. It's a basic Worker service that runs on an interval defined in the *appsettings* file. It also checks for working times and if it's a weekend. All of which could be moved to appsettings of course. It then uses the SendGrid nuget package and SendGrid functionality to actually send the emails.

## Gotchas ##

It can be troublesome with user secrets and docker, but how I used it for my use case was in Azure running Container Instances, you can on setup pass in secrets to the container.


