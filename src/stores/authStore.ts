// stores/authStore.ts

import { API_BASE_URL } from '../../const';


// import { create } from 'zustand';
// import axios from 'axios';
// import Cookies from 'js-cookie';

// export interface User {
//   id: string;
//   name: string;
//   login: string;
//   libraryName: string;
//   readingRoomName: string;
//   role: number;
// }

// interface AuthState {
//   user: User | null;
//   token: string | null;
//   isAuthenticated: boolean;
//   checkAuth: () => void;
//   login: (username: string, password: string) => Promise<void>;
//   fetchUserInfo: () => Promise<void>;
//   logout: () => void;
// }

// export const useAuthStore = create<AuthState>((set) => ({
//   user: null,
//   token: null,
//   isAuthenticated: false,

//   login: async (username, password) => {
//     try {
//       const { data } = await axios.post('http://localhost:5251/Librarians/loginLibrarian', { login: username, password });
//       Cookies.set('some-cookie', data.token);
//       set({ token: data.token, isAuthenticated: true });
//       await useAuthStore.getState().fetchUserInfo();
//     } catch (error) {
//       console.error('Login error:', error);
//       throw error;
//     }
//   },

//   fetchUserInfo: async () => {
//     try {
//       const { data } = await axios.post('http://localhost:5251/Librarians/getWorkerInfo', null, {
//         headers: {
//           Authorization: `Bearer ${Cookies.get('some-cookie')}`, // Добавляем токен из куки
//         },
//       });
//       set({ user: data });
//     } catch (error) {
//       console.error('Fetch user info error:', error);
//     } 
//   },

//   checkAuth: async () => {
//     const token = Cookies.get('some-cookie');
//     if (!token) {
//       set({ isAuthenticated: false, user: null });
//       return;
//     }
//     await useAuthStore.getState().fetchUserInfo(); // Получаем информацию о пользователе
//   },

//   logout: () => {
//     set({ user: null, token: null, isAuthenticated: false });
//     Cookies.remove('some-cookie'); 
//   },
  
// }));

// const token = Cookies.get('some-cookie');
// if (token) {
//   useAuthStore.getState().fetchUserInfo(); // Загружаем информацию о пользователе при старте
//   useAuthStore.getState().checkAuth()
// }

import { create } from 'zustand';
import axios from 'axios';
import Cookies from 'js-cookie';

export interface User {
  id: string;
  name: string;
  login: string;
  libraryName: string;
  readingRoomName: string;
  role: number;
}

interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  checkAuth: () => void;
  login: (username: string, password: string) => Promise<void>;
  fetchUserInfo: () => Promise<void>;
  logout: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: null,
  isAuthenticated: false,

  login: async (username, password) => {
    try {
      const { data } = await axios.post(`${API_BASE_URL}/Librarians/loginLibrarian`, { login: username, password });
      Cookies.set('some-cookie', data.token);
      set({ token: data.token, isAuthenticated: true });
      await useAuthStore.getState().fetchUserInfo();
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  },

  fetchUserInfo: async () => {
    try {
      const { data } = await axios.post(`${API_BASE_URL}/Librarians/getWorkerInfo`, null, {
        headers: {
          Authorization: `Bearer ${Cookies.get('some-cookie')}`, // Добавляем токен из куки
        },
      });
      set({ user: data });
    } catch (error) {
      console.error('Fetch user info error:', error);
    } 
  },

  checkAuth: async () => {
    const token = Cookies.get('some-cookie');
    if (!token) {
      set({ isAuthenticated: false, user: null });
      return;
    }
    await useAuthStore.getState().fetchUserInfo(); // Получаем информацию о пользователе
  },

  logout: () => {
    set({ user: null, token: null, isAuthenticated: false });
    Cookies.remove('some-cookie'); 
  },
  
}));

const token = Cookies.get('some-cookie');
if (token) {
  useAuthStore.getState().fetchUserInfo(); // Загружаем информацию о пользователе при старте
  useAuthStore.getState().checkAuth()
}