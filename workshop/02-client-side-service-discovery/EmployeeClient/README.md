## How to run ?

```
$docker image build -t employee-client .
$docker container run --rm --link employee:employee employee-client:latest
```

Open url=http://localhost:8080/ in browser