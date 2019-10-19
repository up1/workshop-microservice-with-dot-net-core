## Step 1 :: Start service

```
$cd 08-metric
$docker image build -t demo-metric .
$docker container run --rm -p 8080:80 --name demo-metric demo-metric
```

Open url in browser
* http://localhost:8080/metrics
* http://localhost:8080/api/hello

## Step 2 :: Start [prometheus](https://prometheus.io/) (metric server)
```
$docker container run -p 9090:9090 --link demo-metric:demo-metric -v $(pwd)/prometheus.yml:/etc/prometheus/prometheus.yml prom/prometheus
```

Open url=http://localhost:9090 in browser

## Step 3 :: Call service in browser again

Open url in browser
* http://localhost:8080/metrics
* http://localhost:8080/api/hello