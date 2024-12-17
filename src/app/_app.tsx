// _app.tsx
import { useEffect } from 'react';
import { useAuthStore } from '../stores/authStore';
import cookies from 'js-cookie';

function MyApp({ Component, pageProps }) {
  const { fetchUserInfo } = useAuthStore();

  useEffect(() => {
    const token = cookies.get('some-cookie');
    if (token) {
      fetchUserInfo();
    }
  }, [fetchUserInfo]);

  return <Component {...pageProps} />;
}

export default MyApp;
