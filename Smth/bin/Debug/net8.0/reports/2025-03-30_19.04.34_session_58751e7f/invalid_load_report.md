> test info



test suite: `nbomber_default_test_suite_name`

test name: `nbomber_default_test_name`

session id: `2025-03-30_19.04.34_session_58751e7f`

> scenario stats



scenario: `register_invalid_load`

  - ok count: `0`

  - fail count: `5498`

  - all data: `0` MB

  - duration: `00:00:55`

load simulations:

  - `inject`, rate: `100`, interval: `00:00:01`, during: `00:01:00`

|step|ok stats|
|---|---|
|name|`global information`|
|request count|all = `5498`, ok = `0`, RPS = `0`|
|latency (ms)|min = `0`, mean = `0`, max = `0`, StdDev = `0`|
|latency percentile (ms)|p50 = `0`, p75 = `0`, p95 = `0`, p99 = `0`|


|step|failures stats|
|---|---|
|name|`global information`|
|request count|all = `5498`, fail = `5498`, RPS = `100`|
|latency (ms)|min = `4043.55`, mean = `4092.01`, max = `4123.28`, StdDev = `13.66`|
|latency percentile (ms)|p50 = `4096`, p75 = `4100.1`, p95 = `4114.43`, p99 = `4116.48`|


> status codes for scenario: `register_invalid_load`



|status code|count|message|
|---|---|---|
|-101|5498|Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:5001)|


