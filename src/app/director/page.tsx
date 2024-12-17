import Link from 'next/link'
import React from 'react'


export default function Director() {
  return (
    <>
        <Link style={{display: 'block'}} href={"/runlibs"}>Библиотекари</Link>
    </>
  )
}