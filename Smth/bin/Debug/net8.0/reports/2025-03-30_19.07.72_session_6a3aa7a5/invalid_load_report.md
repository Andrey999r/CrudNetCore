> test info



test suite: `nbomber_default_test_suite_name`

test name: `nbomber_default_test_name`

session id: `2025-03-30_19.07.72_session_6a3aa7a5`

> scenario stats



scenario: `register_invalid_load`

  - ok count: `0`

  - fail count: `5500`

  - all data: `0` MB

  - duration: `00:00:55`

load simulations:

  - `inject`, rate: `100`, interval: `00:00:01`, during: `00:01:00`

|step|ok stats|
|---|---|
|name|`global information`|
|request count|all = `5500`, ok = `0`, RPS = `0`|
|latency (ms)|min = `0`, mean = `0`, max = `0`, StdDev = `0`|
|latency percentile (ms)|p50 = `0`, p75 = `0`, p95 = `0`, p99 = `0`|


|step|failures stats|
|---|---|
|name|`global information`|
|request count|all = `5500`, fail = `5500`, RPS = `100`|
|latency (ms)|min = `4038.39`, mean = `4089.59`, max = `4118.65`, StdDev = `14.67`|
|latency percentile (ms)|p50 = `4089.86`, p75 = `4100.1`, p95 = `4114.43`, p99 = `4116.48`|


> status codes for scenario: `register_invalid_load`



|status code|count|message|
|---|---|---|
|-101|5500|Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:5001)|


