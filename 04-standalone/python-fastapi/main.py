from fastapi import FastAPI, Request, status
from fastapi.responses import HTMLResponse, FileResponse, RedirectResponse, JSONResponse
from fastapi.staticfiles import StaticFiles
import random
from datetime import datetime, timedelta
import uvicorn
import logging


class Weather():
    def __init__(self, date, temperatureC, summary):
        self.date = date
        self.temperatureC = temperatureC
        self.summary = summary

summaries=[ "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" ]


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI()

app.mount("/static", StaticFiles(directory="static"), name="static")

@app.get("/", response_class=HTMLResponse)
async def index(request: Request):
    return """
    <html lang="en">
        <head>
            <title>.NET Aspire</title>
            <meta charset="utf-8">
        </head>
        <body style="text-align: center">
            <div><img src="/static/img/python.svg" alt="Python Logo"></div>
            <h1>.NET Aspire</h1>
            <p>Welcome to the FastAPI application.</p>
            <p><a href="/api/weatherforecast">Get a random weather forecast</a></p>
        </body>
    </html>
    """

@app.get("/api/weatherforecast", response_class=JSONResponse)
async def index(request: Request):
    Weathers = [
        Weather((datetime.today() - timedelta(days=1)).date(), random.randint(-20, 55), random.choice (summaries)),
        Weather((datetime.today() - timedelta(days=2)).date(), random.randint(-20, 55), random.choice (summaries)),
        Weather((datetime.today() - timedelta(days=3)).date(), random.randint(-20, 55), random.choice (summaries)),
        Weather((datetime.today() - timedelta(days=4)).date(), random.randint(-20, 55), random.choice (summaries)),
        Weather((datetime.today() - timedelta(days=5)).date(), random.randint(-20, 55), random.choice (summaries))
    ]
    return Weathers

@app.get('/favicon.ico')
async def favicon():
    file_name = 'favicon.ico'
    file_path = './static/' + file_name
    return FileResponse(path=file_path, headers={'mimetype': 'image/vnd.microsoft.icon'})

if __name__ == '__main__':
    uvicorn.run('main:app', host='0.0.0.0', port=8001)
