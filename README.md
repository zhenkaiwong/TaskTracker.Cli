# TaskTracker.Cli

A repository for my personal project "Task Tracker" from roadmap.sh Backend Beginner roadmap

# How to use this?

1. Clone this project to local
2. Run the following commands

## Commands

### Adding a new task

```
dotnet run add "Buy groceries"
```

### Output: Task added successfully (ID: 1)

### Updating and deleting tasks

```
dotnet run update 1 "Buy groceries and cook dinner"
dotnet run delete 1
```

### Marking a task as in progress or done

```
dotnet run mark-in-progress 1
dotnet run mark-done 1
```

### Listing all tasks

```
dotnet run list
```

### Listing tasks by status

```
dotnet run list done
dotnet run list todo
dotnet run list in-progress
```

# URL to this project

https://roadmap.sh/projects/task-tracker
