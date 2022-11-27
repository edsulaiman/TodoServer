# TodoServer

PT. Lingkar Kreasi Flutter Developer Test Server

## Test Description

- Create simple Todo app using Flutter 3+
- App must have at least 2 screen (list, and detail)
- App can add new task
- App can add new subtask
- App can update existing task
- App can delete task
- App can update task is done status on list screen
- App can show task and its subtasks using tree view

## Instalation

- Download [.NET 7+ SDK](https://dotnet.microsoft.com/en-us/download)
- Clone this repository
- Build the app using `dotnet publish -c Release`
- Run app by opening publish folder `bin/Release/net7.0/`

## Changing url

Change `Urls` in `appsettings.json`

## Endpoints

### GET /

Response sample

```json
{
  "message": "Success",
  "data": [
    {
      "id": 1044323137107263488,
      "parentId": null,
      "taskName": "Hello world",
      "isDone": false,
      "startDate": null,
      "dueDate": null,
      "starred": false,
      "createdAt": 1669056438473,
      "updatedAt": null
    }
  ]
}
```

### POST /

Request sample

```json
{
  "parentId": null,
  "taskName": "Hello world",
  "startDate": null,
  "dueDate": null
}
```

Response sample

```json
{
  "message": "Success",
  "data": {
    "id": 1044322029664206848,
    "parentId": null,
    "taskName": "Hello world",
    "isDone": false,
    "startDate": null,
    "dueDate": null,
    "starred": false,
    "createdAt": 1669056174438,
    "updatedAt": null
  }
}
```

### PUT /

Requst sample

```json
{
  "id": 1044323137107263488,
  "parentId": null,
  "taskName": "Hello world!",
  "isDone": false,
  "startDate": null,
  "dueDate": null,
  "starred": false,
  "createdAt": 1669056438473,
  "updatedAt": null
}
```

Response sample

```json
{
  "message": "Success",
  "data": {
    "id": 1044323137107263488,
    "parentId": null,
    "taskName": "Hello world!",
    "isDone": false,
    "startDate": null,
    "dueDate": null,
    "starred": false,
    "createdAt": null,
    "updatedAt": 1669056508532
  }
}
```

### DELETE /{id}

Response sample

```json
{
  "message": "Success"
}
```
