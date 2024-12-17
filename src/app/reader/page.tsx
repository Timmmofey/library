'use client';
import Link from 'next/link';
import React from 'react'

export default function Reader() {
  return (
    <>
        <Link 
          href="/readersloans" 
          className="block w-full h-48 rounded-lg overflow-hidden bg-cover bg-center relative"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=21edbc63d9860ea525b689d19872e3493c7f0123-10576314-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Выдачи изданий
          </div>
        </Link>

        <Link 
          href="/catalog" 
          className="block w-full h-48 rounded-lg overflow-hidden bg-cover bg-center relative mt-4"
          style={{ backgroundImage: "url('https://avatars.mds.yandex.net/i?id=ac0ae73654b7281fa644e26e41288a839dd87b31-5216269-images-thumbs&n=13')" }}
        >
          <div className="absolute bottom-4 left-4 text-white text-lg font-semibold">
            Каталог
          </div>
        </Link>
    </>
  )
}
