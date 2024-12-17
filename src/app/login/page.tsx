"use client"
import { useState } from 'react';
import { useRouter } from "next/navigation";
import { Form, Input, Button, message } from 'antd';
import { useAuthStore } from '../../stores/authStore';
import Link from 'next/link';

const LoginPage = () => {
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const login = useAuthStore((state) => state.login);

  const onFinish = async (values: { username: string; password: string }) => {
    setLoading(true);
    try {
      await login(values.username, values.password);
      message.success('Login successful');
      router.push('/dashboard'); // Перенаправление на основную страницу
    } catch (error) {
        console.error(error)
      message.error('Invalid login or password');
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <div className="relative">
        {/* Добавляем картинку с ограничением размера 250x250 */}
        <img 
          style={{marginTop: '4rem'}}
          src="https://avatars.mds.yandex.net/i?id=04141309ae9b77e7fbf2652fdb78dccf_l-5178044-images-thumbs&n=13" // Замените на URL вашей картинки
          alt="Login Banner"
          className="w-64 h-64 object-cover rounded-t-lg mx-auto" // Ограничение по ширине и высоте 250px
        />
        <Form onFinish={onFinish} layout="vertical" style={{ maxWidth: 300, margin: 'auto', marginTop: '4rem' }}>
          <Form.Item name="username" label="Логин" rules={[{ required: true, message: 'Please input your username!' }]}>
            <Input />
          </Form.Item>
          <Form.Item name="password" label="Пароль" rules={[{ required: true, message: 'Please input your password!' }]}>
            <Input.Password />
          </Form.Item>
          <Button type="primary" htmlType="submit" loading={loading} block>
            Войти
          </Button>
        </Form>
        <Link href={"/readerLogin"} className="block text-center mt-4 text-blue-500">
          Читателям
        </Link>
      </div>
    </>
  );
};

export default LoginPage;
