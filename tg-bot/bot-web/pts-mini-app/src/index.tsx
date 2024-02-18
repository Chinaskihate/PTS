import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './ui/App';
import {AppContextProvider} from './ui/providers/AppProvider';
import {DisplayGate, SDKProvider} from "@tma.js/sdk-react";
import {SDKProviderLoading} from "./ui/components/SDKProviderLoading";
import {SDKInitialState} from "./ui/components/SDKInitialState";
import {SDKProviderError} from "./ui/components/SDKProviderError";


const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

root.render(
    <SDKProvider options={{async: true, complete: true}}>
        <DisplayGate
            error={SDKProviderError}
            loading={SDKProviderLoading}
            initial={SDKInitialState}
        >
            <AppContextProvider>
                <App/>
            </AppContextProvider>
        </DisplayGate>
    </SDKProvider>
);
