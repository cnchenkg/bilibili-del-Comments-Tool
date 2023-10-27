   (function() {
        var i = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
          , V = {
            rotl: function(N, U) {
                return N << U | N >>> 32 - U
            },
            rotr: function(N, U) {
                return N << 32 - U | N >>> U
            },
            endian: function(N) {
                if (N.constructor == Number)
                    return V.rotl(N, 8) & 16711935 | V.rotl(N, 24) & 4278255360;
                for (var U = 0; U < N.length; U++)
                    N[U] = V.endian(N[U]);
                return N
            },
            randomBytes: function(N) {
                for (var U = []; N > 0; N--)
                    U.push(Math.floor(Math.random() * 256));
                return U
            },
            bytesToWords: function(N) {
                for (var U = [], R = 0, Z = 0; R < N.length; R++,
                Z += 8)
                    U[Z >>> 5] |= N[R] << 24 - Z % 32;
                return U
            },
            wordsToBytes: function(N) {
                for (var U = [], R = 0; R < N.length * 32; R += 8)
                    U.push(N[R >>> 5] >>> 24 - R % 32 & 255);
                return U
            },
            bytesToHex: function(N) {
                for (var U = [], R = 0; R < N.length; R++)
                    U.push((N[R] >>> 4).toString(16)),
                    U.push((N[R] & 15).toString(16));
                return U.join("")
            },
            hexToBytes: function(N) {
                for (var U = [], R = 0; R < N.length; R += 2)
                    U.push(parseInt(N.substr(R, 2), 16));
                return U
            },
            bytesToBase64: function(N) {
                for (var U = [], R = 0; R < N.length; R += 3)
                    for (var Z = N[R] << 16 | N[R + 1] << 8 | N[R + 2], W = 0; W < 4; W++)
                        R * 8 + W * 6 <= N.length * 8 ? U.push(i.charAt(Z >>> 6 * (3 - W) & 63)) : U.push("=");
                return U.join("")
            },
            base64ToBytes: function(N) {
                N = N.replace(/[^A-Z0-9+\/]/ig, "");
                for (var U = [], R = 0, Z = 0; R < N.length; Z = ++R % 4)
                    Z != 0 && U.push((i.indexOf(N.charAt(R - 1)) & Math.pow(2, -2 * Z + 8) - 1) << Z * 2 | i.indexOf(N.charAt(R)) >>> 6 - Z * 2);
                return U
            }
        };
        crypt.exports = V
    }
    )();
 
 
 var cryptExports = crypt.exports
      , charenc = {
        utf8: {
            stringToBytes: function(i) {
                return charenc.bin.stringToBytes(unescape(encodeURIComponent(i)))
            },
            bytesToString: function(i) {
                return decodeURIComponent(escape(charenc.bin.bytesToString(i)))
            }
        },
        bin: {
            stringToBytes: function(i) {
                for (var V = [], N = 0; N < i.length; N++)
                    V.push(i.charCodeAt(N) & 255);
                return V
            },
            bytesToString: function(i) {
                for (var V = [], N = 0; N < i.length; N++)
                    V.push(String.fromCharCode(i[N]));
                return V.join("")
            }
        }
    }
      , charenc_1 = charenc;
 
 R = function(Z, W)
 {
            Z.constructor == String ?
           W && W.encoding === "binary" ? Z = U.stringToBytes(Z):（在这里） Z = V.stringToBytes(Z)): 
          N(Z) ? Z = Array.prototype.slice.call(Z, 0): !Array.isArray(Z) && Z.constructor !== Uint8Array && (Z = Z.toString());
            for (var F = i.bytesToWords(Z), z = Z.length * 8, Y = 1732584193, Q = -271733879, J = -1732584194, D = 271733878, X = 0; X < F.length; X++)
                F[X] = (F[X] << 8 | F[X] >>> 24) & 16711935 | (F[X] << 24 | F[X] >>> 8) & 4278255360;
            F[z >>> 5] |= 128 << z % 32,
            F[(z + 64 >>> 9 << 4) + 14] = z;
            for (var P = R._ff, L = R._gg, ae = R._hh, ee = R._ii, X = 0; X < F.length; X += 16) 
            {
                var ie = Y
                  , ue = Q
                  , pe = J
                  , de = D;
                Y = P(Y, Q, J, D, F[X + 0], 7, -680876936),
                D = P(D, Y, Q, J, F[X + 1], 12, -389564586),
                J = P(J, D, Y, Q, F[X + 2], 17, 606105819),
                Q = P(Q, J, D, Y, F[X + 3], 22, -1044525330),
                Y = P(Y, Q, J, D, F[X + 4], 7, -176418897),
                D = P(D, Y, Q, J, F[X + 5], 12, 1200080426),
                J = P(J, D, Y, Q, F[X + 6], 17, -1473231341),
                Q = P(Q, J, D, Y, F[X + 7], 22, -45705983),
                Y = P(Y, Q, J, D, F[X + 8], 7, 1770035416),
                D = P(D, Y, Q, J, F[X + 9], 12, -1958414417),
                J = P(J, D, Y, Q, F[X + 10], 17, -42063),
                Q = P(Q, J, D, Y, F[X + 11], 22, -1990404162),
                Y = P(Y, Q, J, D, F[X + 12], 7, 1804603682),
                D = P(D, Y, Q, J, F[X + 13], 12, -40341101),
                J = P(J, D, Y, Q, F[X + 14], 17, -1502002290),
                Q = P(Q, J, D, Y, F[X + 15], 22, 1236535329),
                Y = L(Y, Q, J, D, F[X + 1], 5, -165796510),
                D = L(D, Y, Q, J, F[X + 6], 9, -1069501632),
                J = L(J, D, Y, Q, F[X + 11], 14, 643717713),
                Q = L(Q, J, D, Y, F[X + 0], 20, -373897302),
                Y = L(Y, Q, J, D, F[X + 5], 5, -701558691),
                D = L(D, Y, Q, J, F[X + 10], 9, 38016083),
                J = L(J, D, Y, Q, F[X + 15], 14, -660478335),
                Q = L(Q, J, D, Y, F[X + 4], 20, -405537848),
                Y = L(Y, Q, J, D, F[X + 9], 5, 568446438),
                D = L(D, Y, Q, J, F[X + 14], 9, -1019803690),
                J = L(J, D, Y, Q, F[X + 3], 14, -187363961),
                Q = L(Q, J, D, Y, F[X + 8], 20, 1163531501),
                Y = L(Y, Q, J, D, F[X + 13], 5, -1444681467),
                D = L(D, Y, Q, J, F[X + 2], 9, -51403784),
                J = L(J, D, Y, Q, F[X + 7], 14, 1735328473),
                Q = L(Q, J, D, Y, F[X + 12], 20, -1926607734),
                Y = ae(Y, Q, J, D, F[X + 5], 4, -378558),
                D = ae(D, Y, Q, J, F[X + 8], 11, -2022574463),
                J = ae(J, D, Y, Q, F[X + 11], 16, 1839030562),
                Q = ae(Q, J, D, Y, F[X + 14], 23, -35309556),
                Y = ae(Y, Q, J, D, F[X + 1], 4, -1530992060),
                D = ae(D, Y, Q, J, F[X + 4], 11, 1272893353),
                J = ae(J, D, Y, Q, F[X + 7], 16, -155497632),
                Q = ae(Q, J, D, Y, F[X + 10], 23, -1094730640),
                Y = ae(Y, Q, J, D, F[X + 13], 4, 681279174),
                D = ae(D, Y, Q, J, F[X + 0], 11, -358537222),
                J = ae(J, D, Y, Q, F[X + 3], 16, -722521979),
                Q = ae(Q, J, D, Y, F[X + 6], 23, 76029189),
                Y = ae(Y, Q, J, D, F[X + 9], 4, -640364487),
                D = ae(D, Y, Q, J, F[X + 12], 11, -421815835),
                J = ae(J, D, Y, Q, F[X + 15], 16, 530742520),
                Q = ae(Q, J, D, Y, F[X + 2], 23, -995338651),
                Y = ee(Y, Q, J, D, F[X + 0], 6, -198630844),
                D = ee(D, Y, Q, J, F[X + 7], 10, 1126891415),
                J = ee(J, D, Y, Q, F[X + 14], 15, -1416354905),
                Q = ee(Q, J, D, Y, F[X + 5], 21, -57434055),
                Y = ee(Y, Q, J, D, F[X + 12], 6, 1700485571),
                D = ee(D, Y, Q, J, F[X + 3], 10, -1894986606),
                J = ee(J, D, Y, Q, F[X + 10], 15, -1051523),
                Q = ee(Q, J, D, Y, F[X + 1], 21, -2054922799),
                Y = ee(Y, Q, J, D, F[X + 8], 6, 1873313359),
                D = ee(D, Y, Q, J, F[X + 15], 10, -30611744),
                J = ee(J, D, Y, Q, F[X + 6], 15, -1560198380),
                Q = ee(Q, J, D, Y, F[X + 13], 21, 1309151649),
                Y = ee(Y, Q, J, D, F[X + 4], 6, -145523070),
                D = ee(D, Y, Q, J, F[X + 11], 10, -1120210379),
                J = ee(J, D, Y, Q, F[X + 2], 15, 718787259),
                Q = ee(Q, J, D, Y, F[X + 9], 21, -343485551),
                Y = Y + ie >>> 0,
                Q = Q + ue >>> 0,
                J = J + pe >>> 0,
                D = D + de >>> 0
            }
            return i.endian([Y, Q, J, D])
        };
        R._ff = function(Z, W, F, z, Y, Q, J) {
            var D = Z + (W & F | ~W & z) + (Y >>> 0) + J;
            return (D << Q | D >>> 32 - Q) + W
        }
        ,
        R._gg = function(Z, W, F, z, Y, Q, J) {
            var D = Z + (W & z | F & ~z) + (Y >>> 0) + J;
            return (D << Q | D >>> 32 - Q) + W
        }
        ,
        R._hh = function(Z, W, F, z, Y, Q, J) {
            var D = Z + (W ^ F ^ z) + (Y >>> 0) + J;
            return (D << Q | D >>> 32 - Q) + W
        }
        ,
        R._ii = function(Z, W, F, z, Y, Q, J) {
            var D = Z + (F ^ (W | ~z)) + (Y >>> 0) + J;
            return (D << Q | D >>> 32 - Q) + W
        }
        ,
        R._blocksize = 16,
        R._digestsize = 16,