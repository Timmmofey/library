"use client"
import { useEffect } from 'react';
import Cookies from 'js-cookie';
import { useAuthStore } from '../stores/authStore';

const useAuthInitialization = () => {
  const { fetchUserInfo, isAuthenticated } = useAuthStore(state => ({
    fetchUserInfo: state.fetchUserInfo,
    isAuthenticated: state.isAuthenticated,
  }));

  useEffect(() => {
    const token = Cookies.get('some-cookie');
    if (token && !isAuthenticated) {
      fetchUserInfo(); // Загружаем информацию о пользователе, если токен существует
    }
  }, [fetchUserInfo, isAuthenticated]);
};

export default useAuthInitialization;
