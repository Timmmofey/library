// stores/authStore.ts
import { create } from 'zustand';
import axios from 'axios';
import Cookies from 'js-cookie';

import { API_BASE_URL } from '../../const';


// export interface Reader {
//     id: string;
//     email: string;
//     fullName: string;
//     libraryId: string;
//     libraryName: string;
//     readerCategoryId: string;
//     subscriptionEndDate: string | null; 
//     educationalInstitution: string | null;
//     faculty: string | null;
//     course: number | null; 
//     groupNumber: string | null;
//     organization: string | null;
//     researchTopic: string | null;
//     role: 3
//   }

// interface AuthState {
//   reader: Reader | null;
//   token: string | null;
//   isReaderAuthenticated: boolean;
//   checkAuth: () => void;
//   loginReader: (username: string, password: string) => Promise<void>;
//   fetchUserInfo: () => Promise<void>;
//   logout: () => void;
// }

// export const useReaderAuthStore = create<AuthState>((set) => ({
//   reader: null,
//   token: null,
//   isReaderAuthenticated: false,

//   loginReader: async (username, password) => {
//     try {
//       const { data } = await axios.post('http://localhost:5251/api/Reader/loginReader', { email: username, password });
//       Cookies.set('some-cookie', data.token);
//       set({ token: data.token, isReaderAuthenticated: true });
//       await useReaderAuthStore.getState().fetchUserInfo();
      
//     } catch (error) {
//       console.error('Login error:', error);
//       throw error;
//     }
//   },

//   fetchUserInfo: async () => {
//     try {
//       const { data } = await axios.post('http://localhost:5251/api/Reader/getReadersInfo', null, {
//         headers: {
//           Authorization: `Bearer ${Cookies.get('some-cookie')}`, // Добавляем токен из куки
//         },
//       });
//       set({ reader: data });
//       console.log(data)
//     } catch (error) {
//       console.error('Fetch reader info error:', error);
//     } 
//   },

//   checkAuth: async () => {
//     const token = Cookies.get('some-cookie');
//     if (!token) {
//       set({ isReaderAuthenticated: false, reader: null });
//       return;
//     }
//     await useReaderAuthStore.getState().fetchUserInfo(); // Получаем информацию о пользователе
//   },

//   logout: () => {
//     set({ reader: null, token: null, isReaderAuthenticated: false });
//     Cookies.remove('some-cookie'); 
//   },
  
// }));

// const token = Cookies.get('some-cookie');
// if (token) {
//   useReaderAuthStore.getState().fetchUserInfo(); // Загружаем информацию о пользователе при старте
//   useReaderAuthStore.getState().checkAuth()
// }


export interface Reader {
  id: string;
  email: string;
  fullName: string;
  libraryId: string;
  libraryName: string;
  readerCategoryId: string;
  subscriptionEndDate: string | null; 
  educationalInstitution: string | null;
  faculty: string | null;
  course: number | null; 
  groupNumber: string | null;
  organization: string | null;
  researchTopic: string | null;
  role: 3
}

interface AuthState {
reader: Reader | null;
token: string | null;
isReaderAuthenticated: boolean;
checkAuth: () => void;
loginReader: (username: string, password: string) => Promise<void>;
fetchUserInfo: () => Promise<void>;
logout: () => void;
}

export const useReaderAuthStore = create<AuthState>((set) => ({
reader: null,
token: null,
isReaderAuthenticated: false,

loginReader: async (username, password) => {
  try {
    const { data } = await axios.post(`${API_BASE_URL}/api/Reader/loginReader`, { email: username, password });
    Cookies.set('some-cookie', data.token);
    set({ token: data.token, isReaderAuthenticated: true });
    await useReaderAuthStore.getState().fetchUserInfo();
    
  } catch (error) {
    console.error('Login error:', error);
    throw error;
  }
},

fetchUserInfo: async () => {
  try {
    const { data } = await axios.post(`${API_BASE_URL}/api/Reader/getReadersInfo`, null, {
      headers: {
        Authorization: `Bearer ${Cookies.get('some-cookie')}`, // Добавляем токен из куки
      },
    });
    set({ reader: data });
    console.log(data)
  } catch (error) {
    console.error('Fetch reader info error:', error);
  } 
},

checkAuth: async () => {
  const token = Cookies.get('some-cookie');
  if (!token) {
    set({ isReaderAuthenticated: false, reader: null });
    return;
  }
  await useReaderAuthStore.getState().fetchUserInfo(); // Получаем информацию о пользователе
},

logout: () => {
  set({ reader: null, token: null, isReaderAuthenticated: false });
  Cookies.remove('some-cookie'); 
},

}));

const token = Cookies.get('some-cookie');
if (token) {
useReaderAuthStore.getState().fetchUserInfo(); // Загружаем информацию о пользователе при старте
useReaderAuthStore.getState().checkAuth()
}