# TaskCaptain

TaskCaptain is a C# desktop application designed to allow specific automated actions on a Todoist account. TaskCaptain has a WPF GUI and uses Todoist REST API's to interact directly with your Todoist account to provide advanced task actions.

## Intended Use

This application is intended as a helper application alongside Todoist, not to replace Todoist itself. The automated actions are designed for quick automation of tasks that would typically take a bit of labor in the regular Todoist application. It won't add any _new_ functionality (yet).

## Automated Actions

#### Create a Task on the Last Workday of Every Month
Create a specified template task on the last workday of each month.

#### Schedule to Week Start
Move all tasks from a week (or weeks) to either the beginning of the week (Monday) or the beginning of the weekend (Saturday).

#### Clear To Inbox
Send all tasks from a Project Back to the Inbox.

#### Translate Recurrence
Translates a recurring task into it's individual recurrences over a specified time interval.

## Focus View
In addition to providing automated tasks, TaskCaptain has a Focus View that brings the view down to a few selected tasks. Similar to the star functionality in Wunderlist, TaskCaptain allows you to select certain tasks to be displayed in Focus View, helping you to quickly decide which tasks you're going to work on and focus on those in the moment.

## Contents

#### TaskCaptain
TaskCaptain contains the code for running the application: both the data model for Todoist objects and the files for running the WPF application.

#### Boatswain
Boatswain contains code for unit testing via Visual Studio.

## Notes
**Version**: 0.2  
**Author**: Sam Govier ([GitHub](https://github.com/samgovier))  
TaskCaptain is not created by, affiliated with, or supported by Doist.