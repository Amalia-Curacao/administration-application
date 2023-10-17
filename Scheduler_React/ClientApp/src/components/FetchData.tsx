import React, { useState, ReactElement } from 'react';

export default async function FetchData() : Promise<ReactElement> {
  const [forecasts, setForecasts] = useState({ forecasts: [], loading: true});

  const response = await fetch('weatherforecast');
  const data = await response.json();
  setForecasts({ forecasts: data, loading: false });
  
  return forecasts.loading ? <p><em>Loading...</em></p> :
  <table className="table table-striped" aria-labelledby="tableLabel">
    <thead>
      <tr>
        <th>Date</th>
        <th>Temp. (C)</th>
        <th>Temp. (F)</th>
        <th>Summary</th>
      </tr>
    </thead>
    <tbody>
      {forecasts.forecasts.map((forecast: any) =>
        <tr key={forecast.date}>
          <td>{forecast.date}</td>
          <td>{forecast.temperatureC}</td>
          <td>{forecast.temperatureF}</td>
          <td>{forecast.summary}</td>
        </tr>
      )}
    </tbody>
  </table>;
}