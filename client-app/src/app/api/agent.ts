import axios, { AxiosError, AxiosResponse } from 'axios';
import toast from 'react-hot-toast';
import { Event } from '../models/event';
import { User, UserFormValues } from '../models/user';
import { router } from '../router/Routes';
import { store } from '../stores/store';

/** Adding a `fake` delay to the app for testing the `loading` indicators after requests. */
const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    });
}

/** Setting up the default url to our API. */
axios.defaults.baseURL = 'https://localhost:7246/api';

/**
 * Setting up an Axios interceptor for computing the response.
 * 
 * In case of an error getting thrown by the API, we also use interceptor for quality of life
 * and user experience.
 */
axios.interceptors.response.use(async response => {
    await sleep(1000);
    return response;
}, (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key])
                        modalStateErrors.push(data.errors[key]);
                }
                throw modalStateErrors.flat();
            } else {
               toast.error(data); 
            }
            break;
        case 401:
            toast.error('Unauthorized');
            break;
        case 403:
            toast.error('Forbidden');
            break;
        case 404:
            /** Navigate the user to our NotFound react component. */
            router.navigate('/not-found');
            break;
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error');
            break;
    }
    return Promise.reject(error);
});

/** Extracting the data from the response. */
const responseBody = <T> (response: AxiosResponse<T>) => response.data;

/**
 * Axios requests for generic CRUD operations to our API.
 * */
const requests = {
    get: <T> (url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T> (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T> (url: string) => axios.delete<T>(url).then(responseBody),
}

/**
 * Event [domain entity] related requests.
 * */
const Events = {
    list: () => requests.get<Event[]>('/events'),
    details: (id: string) => requests.get<Event>(`/events/${id}`),
    create: (event: Event) => requests.post<void>('/events/', event),
    update: (event: Event) => requests.put<void>(`/events/${event.id}`, event),
    delete: (id: string) => requests.del<void>(`/events/${id}`)
}

/**
 * Account related requests. 
 */
const Accounts = {
    login: (user: UserFormValues) => requests.post<User>('/accounts/login', user)
}

/**
 * Building the `agent` object.
 * */
const agent = {
    Accounts,
    Events
}

/**
 * Exporting the `agent` object that serves as our proxy to our API.
 * */
export default agent;