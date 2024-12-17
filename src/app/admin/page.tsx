"use client"
import Header from '@/components/Header'
import React, { useEffect } from 'react'
// import LibrariansPage from '../adddirector/page'
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/stores/authStore';
import Link from 'next/link';


export default function Admin() {

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
          router.push('/login'); // Если роль неизвестна, остаемся на главной
          break;
      }
    } else {
      router.push('/login');
    }
  }, [user]);

  return (
    <>
        {/* <Header></Header> */}
        {/* <Link style={{display: 'block'}} href={"/adddirector"}>Управление директорами</Link>
        <Link style={{display: 'block'}} href={"/runlibraries"}>Управление библиотеками</Link>
        <Link style={{display: 'block'}} href={"/runreadingrooms"}>Управление читальными залами</Link>
        <Link style={{display: 'block'}} href={"/adminstats"}>Статистика</Link> */}

        <Link 
          href="/adddirector" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=cf1c4c7b24292414a4fdbbca696e1f0610a41c53-10266303-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Управление директорами
          </div>
        </Link>
        <Link 
          href="/runlibraries" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=6baf42f736a6ffc46feec510ed881ec967831a0c-4829304-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Управление библиотеками
          </div>
        </Link>
        <Link 
          href="/runreadingrooms" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=5b2d3d4435bd102ac72517e5935fb095e2fbea8c-10698872-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Управление читальными залами
          </div>
        </Link>
        <Link 
          href="/adminstats" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=3ab0d1183c4ef4a1761b3477ca26d9ee131521ae-4885380-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Статистика
          </div>
        </Link>
    </>
  )
}