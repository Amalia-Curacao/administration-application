import React from 'react';
import ReactDOM from 'react-dom/client';
import 'bootstrap/dist/css/bootstrap.css';
import reportWebVitals from './reportWebVitals';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import ScheduleIndex, { link as ScheduleIndexLink} from './pages/schedule';
import Layout from './layout';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

// First link is the default page
const router = createBrowserRouter([
  {
    path: '/',
    element: <ScheduleIndex/>
  },
  {
    path: ScheduleIndexLink,
    element: <ScheduleIndex/>
  },
]);

root.render(
  <React.StrictMode>
    <Layout>
      <RouterProvider router={router}/>
    </Layout>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
