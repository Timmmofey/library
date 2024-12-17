"use client"
import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuthStore } from '../../stores/authStore';
import { Spin } from 'antd';  // Импортируем Spin компонент из Ant Design

const Dashboard = () => {
  const router = useRouter();
  const { user, isAuthenticated } = useAuthStore();

  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
    } else if (user) {
      if (user.role === 2) router.push('/librarian');
      else if (user.role === 0) router.push('/admin');
      else if (user.role === 1) router.push('/director');
    }
  }, [isAuthenticated, user, router]);

  return (
    <div className="flex justify-center items-center min-h-screen">
      {/* Добавляем компонент загрузки */}
      <Spin size="large" />
    </div>
  );
};

export default Dashboard;
