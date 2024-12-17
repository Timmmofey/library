"use client"
import { useState } from 'react';
import { useRouter } from "next/navigation";
import { Form, Input, Button, message } from 'antd';
import { useReaderAuthStore } from "../../stores/readerAuthStore";
import Link from 'next/link';

const LoginPage = () => {
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const loginReader = useReaderAuthStore((state) => state.loginReader);

  const onFinishReader = async (values: { username: string; password: string }) => {
    setLoading(true);
    try {
      await loginReader(values.username, values.password);
      message.success('Login successful');
      router.push('/reader'); // Перенаправление на основную страницу
    } catch (error) {
      console.error(error);
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
          src="https://avatars.mds.yandex.net/i?id=04141309ae9b77e7fbf2652fdb78dccf_l-5178044-images-thumbs&n=13" // Замените на URL вашей картинки
          alt="Login Banner"
          className="w-64 h-64 object-cover rounded-t-lg mx-auto mt-16" // Ограничение по ширине и высоте 250px
        />
        <Form
          onFinish={onFinishReader}
          layout="vertical"
          style={{ maxWidth: 300, margin: 'auto', marginTop: '4rem' }}
        >
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
        <div className="text-center mt-4">
          <Link href={"/register"} className="block text-blue-500 mb-2">Регистрация</Link>
          <Link href={"/login"} className="block text-blue-500">Сотрудникам</Link>
        </div>
      </div>
    </>
  );
};

export default LoginPage;
