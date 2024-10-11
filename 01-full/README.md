.NET Aspire Full Feature Demo
=============================

This sample includes a gratuitous amount of bells and whistles to showcase many features of .NET Aspire.


Usage
-----

1. Ensure you have Docker Desktop or Podman installed

2. Ensure you have .NET Aspire [installed](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling).

3. In `vue-app` folder run `npm install`.  There are ways to do this auto-magically in .NET Aspire but they require [MS Build magic](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/build-aspire-apps-with-nodejs#explore-the-app-host).

4. Open `AspireFull.sln`.

5. Start debugging.

6. When the .NET Aspire dashboard pulls up, explore each application, the configuration, and logs.

7. Switch to the Traces tab, and as you click through applications, note the detailed call stacks in each trace.

8. Switch to Structured Logs and for each entry note not just the message but also the exploded view below.


Interesting features
--------------------

1. As the .NET Aspire App Host spins up, it initializes the Postgres database with a table and seed data.  The app uses the `alpine` image label.

2. The .NET Aspire App Host spins up a Redis container used for output caching in both .NET apps.  We use the `alpine` image label here too.

3. The backend API is a minimal API that uses output caching in Redis and queries a Postgres database using Entity Framework.

4. A .NET frontend app is a Razor Pages app and uses output caching in Redis.

5. In the ServiceDefaults project, more tracing instrumentation libraries are configured to harvest database and API calls.

6. The .NET frontend and backend share an `Entities` project to ensure they always have up-to-date details of the API.

7. A second frontend is a Vue.js app that also queries the API.  Vite proxies `/api/` to the backend API to avoid CORS issues, and ensures it starts on the port designated by .NET Aspire.  Client-side OpenTelemetry is built into `src/tracing.ts` that forwards to the .NET Aspire dashboard.
