# Mako-IoT.Samples
- [Messaging]() -- TODO
- [Configuration API](https://github.com/CShark-Hub/Mako-IoT.Samples/tree/main/ConfigurationAPI)
- [Log Storage](https://github.com/CShark-Hub/Mako-IoT.Samples/tree/main/LogStorage)
- [Mediator](https://github.com/CShark-Hub/Mako-IoT.Samples/tree/main/Mediator)
- [Waste Bins Calendar (full product example)](https://github.com/CShark-Hub/Mako-IoT.Samples/tree/main/WasteBinsCalendar)

TODO: descriptions

## How to manually sync fork
- Clone repository and navigate into folder
- From command line execute bellow commands
- **git remote add upstream https://github.com/CShark-Hub/Mako-IoT.Base.git**
- **git fetch upstream**
- **git rebase upstream/main**
- If there are any conflicts, resolve them
  - After run **git rebase --continue**
  - Check for conflicts again
- **git push -f origin main**
