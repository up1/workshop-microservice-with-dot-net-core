## How to run ?

```
$docker image build -t service_a .
$docker container run --rm -p 7333:7333 service_a
```

Open url=http://localhost:7333/ in browser