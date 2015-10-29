On Sqlite when do migration on dnx RC1 we have error so to fix it after 
```
dnu restore
```
find in project.lock.json file sqlite and change native sections to:
```json
 "native": {
          "runtimes/win/native/x64/sqlite3.dll": {},
          "runtimes/win/native/x64/x86/sqlite3.dll": {},
          "runtimes/win/native/x86/sqlite3.dll": {}
        }
```
to
```json
"native": {
          "runtimes/win/native/x64/sqlite3.dll": {}
        }
```