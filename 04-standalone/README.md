.NET Aspire Dashboard Stand-alone
=================================

This demo shows how the .NET Aspire dashboard can be used as a stand-alone container without an AppHost project.  It shows both containerized and locally debugged apps piping OpenTelemetry data into the .NET Aspire dashboard.

Though we get traces, some logs, and metrics, we don't get service discovery.  So the initial tab showing all the URLs is missing.


Usage
-----

### Before everything: Launch Dashboard container

Spin up the .NET Aspire dashboard and supporting data stores:

```sh
docker compose up
```

Browse to http://localhost:8000/ to see the .NET Aspire dashboard.  As you run each app, you can see some console logs and lots of traces from each source.

Browse to http://localhost:8081/ to see the Postgres Admin tool.  From there you can connect to the Postgres database and see the weatherforecast table.

Both Postgres and Redis are also running.

See interesting details in `docker-compose.yaml` including:

- environment variables to start the .NET Aspire dashboard container
- starting Postgres and initializing a database
- starting a Postgres admin tool
- starting Redis


### Run apps locally

One way to use the .NET Aspire dashboard is with locally running apps.


#### .NET Apps

1. Launch `AspireStandalone.sln` in Visual Studio or VS Code.

2. In Solution Explorer, right-click on one of the projects and choose `Manage User Secrets`.

3. Set the secrets.json file to match the details in `docker-compose.yaml`:

   ```json
   {
     "ApiServiceUrl": "https://localhost:7364",
     "ConnectionStrings": {
       "Redis": "localhost:6379",
       "Postgres": "Host=localhost;Port=5432;Username=postgres;Password=Pa55word.;Database=weatherforecast"
     }
   }
   ```

4. In the Solution Explorer, right-click on `Solution AspireStandalone`, choose `Configure Startup Projects`, click `Multiple Startup projects`, and set both projects to `Start`.

5. Start debugging.

6. Browse to https://localhost:7364/swagger to see the API.

7. See the console output from the load test project as it queries both API endpoints.

See interesting details in:

- `Properties/launchSettings.json` has environment variables that mirror settings in docker-compose.yaml. In a production app
- `Program.cs` configures many more trace providers to show details for HttpClient and data store calls


#### Vue.js app

1. Open a terminal in the `vue-app` folder.

2. Install dependencies.  Run `npm install`.

3. Start the app.  Run `npm run dev`.

4. Browse to http://localhost:3000/

**Note:** This app proxies to the .NET API, so that API needs to be running first.

See interesting details in:

- `vite.config.ts` reads the environment variable for the server URL.
- `src/tracing.ts` loads up client-side OpenTelemetry configuration.


#### Python API

1. Open a terminal in the `python-fastapi` folder.

2. Create a virtual environment.  Run `python -m venv .venv`.

3. Activate the virtual environment.

   Windows Powershell: `.\.venv\Scripts\Activate.ps1`

   Linux or macOS: `source .venv/bin/activate`

4. Install dependencies.  Run `pip install`.

5. Start the app.

   Windows Powershell: `start.ps1`

   Linux and macOS: `start.sh`

6. Browse to http://localhost:8001/

**Note:** No clients connect to this backend, but you can still flex the weather API and see the traces in the .NET Aspire dashboard.

See interesting details in:

- `start.sh` and `start.ps1` loads up all the OpenTelemetry magic without touching the app.
- `requirements.txt` has `opentelemetry-distro[otlp]` which loads up the `opentelemetry-instrument` app.


### Run apps in containers

Build and run all the apps in containers:

```sh
docker compose -f docker-compose.apps.yaml up
```

#### .NET Apps

Browse to the .NET API at http://localhost:8080/api/weatherforecast.

See interesting details in:

- `docker-compose.apps.yaml` has all the data store and other config as well as OpenTelemetry setup needed.


#### Vue.js app

Browse to the Vue.js app at http://localhost:8082/.  Note how the dashboard logs the Nginx proxy sending the home page to the Vue.js app and the api request to the .NET API.

See interesting details in:

- `Dockerfile` installs OpenTelemetry dependencies into Nginx.
- `nginx.conf` logs server details into OpenTelemetry and proxies api requests to the .NET backend.


#### Python API

Browse to the Python / FastAPI app at http://localhost:8001/.  No clients connect to this API, but you can still flex the weather API and see the traces in the .NET Aspire dashboard.

See interesting details in:

- `Dockerfile` mirrors `start.sh` and `start.ps1`.
- `docker-compose.apps.yaml` adds the OpenTelemetry config.
