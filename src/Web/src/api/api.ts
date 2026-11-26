// src/api/api.ts
import axios, { AxiosInstance } from "axios";
import Logger from "../utility/Logger";

const PORT = import.meta.env.VITE_PORT || 8000;
const API_BASE =
  import.meta.env.VITE_SERVER_IP ||
  `${location.protocol}//${location.hostname}:${PORT}`;
let accessToken: string | null = null;

const api: AxiosInstance = axios.create({
  baseURL: API_BASE,
  timeout: 15000,
  withCredentials: true,
});

export function setAccessToken(token: string | null) {
  accessToken = token;

  if (token) {
    api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common["Authorization"];
  }
}

export function getAccessToken() {
  return accessToken;
}

export function clearAccessToken() {
  accessToken = null;
  delete api.defaults.headers.common["Authorization"];
}

export const send = {
  get: async (endpoint: string) => {
    try {
      return await api.get(endpoint);
    } catch (e: any) {
      Logger.error(e);
      return e.response;
    }
  },

  getImage: async (endpoint: string) => {
    try {
      return await api.get(endpoint, {
        responseType: "arraybuffer",
      });
    } catch (e: any) {
      Logger.error(e);
      return e.response;
    }
  },

  post: async (endpoint: string, payload?: Object) => {
    try {
      return await api.post(endpoint, payload);
    } catch (e: any) {
      Logger.error(e);
      return e;
    }
  },

  put: async (endpoint: string, payload: Object) => {
    try {
      return await api.put(endpoint, payload);
    } catch (e: any) {
      Logger.error(e);
      return e.response;
    }
  },

  delete: async (endpoint: string) => {
    try {
      return await api.delete(endpoint);
    } catch (e: any) {
      Logger.error(e);
      return e.response;
    }
  },

  deleteBody: async (endpoint: string, payload: Object) => {
    try {
      return await api.delete(endpoint, {
        data: payload,
      });
    } catch (e: any) {
      Logger.error(e);
      return e.response;
    }
  },

  postForm: async (endpoint: string, payload: FormData) => {
    try {
      return await api.post(endpoint, payload);
    } catch (e: any) {
      Logger.error(e);
      return e.response;
    }
  },
};

api.interceptors.request.use((config) => {
  const isFormData = config.data instanceof FormData;

  if (accessToken) {
    config.headers.set("Authorization", `Bearer ${accessToken}`);
  }

  if (isFormData) {
    // Let Axios/browser set multipart/form-data with the boundary
    config.headers.delete("Content-Type");
  } else if (!config.headers.has("Content-Type")) {
    config.headers.set("Content-Type", "application/json");
  }

  return config;
});

export default api;