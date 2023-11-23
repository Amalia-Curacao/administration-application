import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import reportWebVitals from './reportWebVitals';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import Layout from './layout';
import RouteObjects from './routes';



createRoot(document.getElementById('root') as HTMLElement).render(
  <StrictMode>
    <Layout>
      <RouterProvider router={createBrowserRouter(RouteObjects())} />
    </Layout>
  </StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
