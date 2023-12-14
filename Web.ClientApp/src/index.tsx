import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import {createRoot} from 'react-dom/client';
import {BrowserRouter} from 'react-router-dom';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import {Provider} from "react-redux";
import store from "./BL/store";
import {App} from "./App";

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
const rootElement = document.getElementById('root')!;
const root = createRoot(rootElement)!;

root.render(
    <Provider store={store}>
        <BrowserRouter basename={baseUrl}>
            <App/>
        </BrowserRouter>);
    </Provider>
);

serviceWorkerRegistration.unregister();

