> test info



test suite: `nbomber_default_test_suite_name`

test name: `nbomber_default_test_name`

session id: `2025-03-30_16.42.42_session_5c79b03a`

> scenario stats



scenario: `register_load`

  - ok count: `0`

  - fail count: `7500`

  - all data: `0` MB

  - duration: `00:00:15`

load simulations:

  - `inject`, rate: `500`, interval: `00:00:01`, during: `00:02:00`

|step|ok stats|
|---|---|
|name|`global information`|
|request count|all = `7500`, ok = `0`, RPS = `0`|
|latency (ms)|min = `0`, mean = `0`, max = `0`, StdDev = `0`|
|latency percentile (ms)|p50 = `0`, p75 = `0`, p95 = `0`, p99 = `0`|


|step|failures stats|
|---|---|
|name|`global information`|
|request count|all = `7500`, fail = `7500`, RPS = `500`|
|latency (ms)|min = `3999.68`, mean = `4001.84`, max = `4023.25`, StdDev = `1.7`|
|latency percentile (ms)|p50 = `4001.79`, p75 = `4003.84`, p95 = `4005.89`, p99 = `4007.94`|


> status codes for scenario: `register_load`



|status code|count|message|
|---|---|---|
|-101|7500|Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:5001)|


