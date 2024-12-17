"use client"
import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '../stores/authStore';



function Home() {
  const router = useRouter();
  const user  = useAuthStore((state) => state.user); // Подключаем состояние

  useEffect(() => {
    console.log(user)
    
    if (user) {
      // Проверяем роль пользователя и перенаправляем
      switch (user.role) {
        case 0: // Пример роли для директора
          router.push('/admin'); // Страница для директора
          break;
        case 1: // Пример роли для библиотекаря
          router.push('/director'); // Страница для библиотекаря
          break;
        case 2: // Пример роли для администратора
          router.push('/librarian'); // Страница для администратора
          break;
        default:
          router.push('/readerLogin'); // Если роль неизвестна, остаемся на главной
          break;
      }
    } else {
      router.push('/readerLogin');
    }
  }, [user]);

  return (
    <h1>Welcome to library</h1>
  );
}

export default Home;