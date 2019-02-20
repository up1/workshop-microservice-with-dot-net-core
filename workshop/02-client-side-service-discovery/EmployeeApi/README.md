## How to run ?

```
$docker image build -t point-to-point .
$docker container run --rm -p 8080:80 --name employee point-to-point
```

Open url=http://localhost:8080/ in browser