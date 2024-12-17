"use client"
import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '../stores/authStore';

const useRoleRedirect = () => {
  const router = useRouter();
  const user = useAuthStore(state => state.user);

  useEffect(() => {
    if (user) {
      switch (user.role) {
        case 0: // Admin
          router.push('/admin');
          break;
        case 1: // Director
            router.push('/director');
            break;
        case 2: // Librarian
          router.push('/librarian');
          break;
        default:
          router.push('/'); // Главная страница для других ролей
          break;
      }
    }
  }, [user, router]); // Следим за изменением user
};

export default useRoleRedirect;
