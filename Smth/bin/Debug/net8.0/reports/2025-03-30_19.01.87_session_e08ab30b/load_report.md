> test info



test suite: `nbomber_default_test_suite_name`

test name: `nbomber_default_test_name`

session id: `2025-03-30_19.01.87_session_e08ab30b`

> scenario stats



scenario: `register_load`

  - ok count: `0`

  - fail count: `7496`

  - all data: `0` MB

  - duration: `00:00:15`

load simulations:

  - `inject`, rate: `500`, interval: `00:00:01`, during: `00:02:00`

|step|ok stats|
|---|---|
|name|`global information`|
|request count|all = `7496`, ok = `0`, RPS = `0`|
|latency (ms)|min = `0`, mean = `0`, max = `0`, StdDev = `0`|
|latency percentile (ms)|p50 = `0`, p75 = `0`, p95 = `0`, p99 = `0`|


|step|failures stats|
|---|---|
|name|`global information`|
|request count|all = `7496`, fail = `7496`, RPS = `499.7`|
|latency (ms)|min = `1.23`, mean = `4062.56`, max = `4112.23`, StdDev = `49.59`|
|latency percentile (ms)|p50 = `4065.28`, p75 = `4077.57`, p95 = `4087.81`, p99 = `4100.1`|


> status codes for scenario: `register_load`



|status code|count|message|
|---|---|---|
|-101|7496|Подключение не установлено, т.к. конечный компьютер отверг запрос на подключение. (localhost:5001)|


