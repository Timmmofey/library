"use client"
import React, { useEffect } from 'react'
import { useRouter } from 'next/navigation';
import { useAuthStore } from '@/stores/authStore';
import Link from 'next/link';



export default function Librarian() {
  
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
        {/* <LoanTable/> */}
        {/* <Link style={{display: 'block'}} href={"/runloans"}>Выдачи изданий</Link>
        <Link style={{display: 'block'}} href={"/runauthors"}>Авторы</Link>
        <Link style={{display: 'block'}} href={"/runitems"}>Издания</Link>
        <Link style={{display: 'block'}} href={"/runitemcopies"}>Копии изданий</Link> */}

        <Link 
          href="/runloans" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=51ba0d8dd0e9103884795547654ddc67731bc835-13290341-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Выдачи изданий
          </div>
        </Link>
        <Link 
          href="/runauthors" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=0dcc3c46be1458fd0ff2d9d5ebdb56caef8076e834cd61e2-10253687-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Авторы
          </div>
        </Link>
        <Link 
          href="/runitems" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=bf07ebc20b499450b1b9079970557824e69e33a5-8805584-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Издания
          </div>
        </Link>
        <Link 
          href="/runitemcopies" 
          className="block w-full mb-3 h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=f7f855dd27933295c21f0a923c50ff4d66ecda9f-10355051-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Копии изданий
          </div>
        </Link>
    </>
  )
}