"use client"
import { useEffect } from 'react';
import { useRouter } from 'next/router';
import { useAuthStore } from '../stores/authStore';
import Cookies from 'js-cookie';

const withAuthRedirect = (WrappedComponent: React.ComponentType) => {
  return function ComponentWithAuth(props: any) {
    const { user, fetchUserInfo } = useAuthStore();
    const router = useRouter();

    useEffect(() => {
      const initialize = async () => {
        const token = Cookies.get('some-cookie');
        if (token) {
          await fetchUserInfo(); // Загружаем данные пользователя
          if (!user) return;

          // Перенаправление на страницу в зависимости от роли
          switch (user.role) {
            case 1: // Администратор
              router.push('/admin');
              break;
            case 2: // Либрариан
              router.push('/librarian');
              break;
            case 3: // Директор
              router.push('/director');
              break;
            default:
              router.push('/unauthorized'); // Ошибка доступа
          }
        } else {
          router.push('/'); // Перенаправляем на страницу логина
        }
      };

      initialize();
    }, [fetchUserInfo, router, user]);

    return <WrappedComponent {...props} />;
  };
};

export default withAuthRedirect;
